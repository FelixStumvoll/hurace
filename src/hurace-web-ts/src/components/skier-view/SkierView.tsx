import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { Skier } from '../../interfaces/Skier';
import { setStateAsync } from '../../common/stateSetter';
import { getSkiers } from '../../api';
import { SkierListItem } from './SkierListItem';

const SkierPanel = styled.div`
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    overflow: hidden;
`;

const SearchBar = styled.input`
    margin: 0 0 10px auto;
    border-radius: 15px;
    padding-left: 10px;
    border: 1px solid ${props => props.theme.gray};
    height: 25px;
    width: 200px;
`;

const ListPanel = styled.div`
    overflow: auto;
`;

const SkierTable = styled.table`
    width: 100%;
    border-collapse: collapse;
`;

export const SkierView: React.FC = () => {
    const [skiers, setSkiers] = useState<Skier[] | undefined>(undefined);

    useEffect(() => {
        if (skiers !== undefined) return;
        setStateAsync(setSkiers, getSkiers());
    }, [skiers]);

    return (
        <SkierPanel>
            <SearchBar placeholder="Suche" />
            <ListPanel>
                <SkierTable>
                    <thead>
                        <tr>
                            <th align="left">Land</th>
                            <th align="left">Name</th>
                            <th align="left">Geschlecht</th>
                        </tr>
                    </thead>
                    <tbody>
                        {skiers?.map(s => (
                            <SkierListItem key={s.id} skier={s} />
                        ))}
                    </tbody>
                </SkierTable>
            </ListPanel>
        </SkierPanel>
    );
};
