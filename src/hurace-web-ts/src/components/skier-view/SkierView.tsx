import React from 'react';
import styled from 'styled-components';
import { Skier } from '../../interfaces/Skier';
import { getSkiers } from '../../common/api';
import { SkierViewItem } from './SkierViewItem';
import { MasterViewWrapper } from '../shared/MasterViewWrapper';
import { SearchContext } from '../shared/MasterViewWrapper';
import { useStateAsync } from '../../hooks/useStateAsync';

const SkierList = styled.div`
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    overflow: auto;
`;

const skierFilter = (skier: Skier, searchTerm: string): boolean =>
    skier.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    skier.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    skier.country.countryName.toLowerCase().includes(searchTerm.toLowerCase());

export const SkierView: React.FC = () => {
    const [skiers] = useStateAsync(getSkiers);

    return (
        <MasterViewWrapper createText="Rennläufer erstellen" createUrl="/skier/new">
            <SearchContext.Consumer>
                {search => (
                    <SkierList>
                        {skiers
                            ?.sort((s1, s2) =>
                                s1.lastName.localeCompare(s2.lastName)
                            )
                            .filter(s => skierFilter(s, search))
                            .map(s => (
                                <SkierViewItem key={s.id} skier={s} />
                            ))}
                    </SkierList>
                )}
            </SearchContext.Consumer>
        </MasterViewWrapper>
    );
};
