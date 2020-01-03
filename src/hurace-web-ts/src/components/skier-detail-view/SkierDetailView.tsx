import React, { useState, useEffect } from 'react';
import { setStateAsync } from '../../common/stateSetter';
import { getSkierById } from '../../api';
import { Skier } from '../../interfaces/Skier';
import styled from 'styled-components';
import { SkierDetailPanel } from './SkierDetailPanel';
import { Card } from '../../theme/StyledComponents';
import { BackLinkWrapper } from '../BackLinkWrapper';

const SkierGrid = styled.div`
    display: flex;
    flex-direction: column;
    column-gap: 10px;
`;

const SkierDetailCard = styled(Card)`
    display: flex;
    padding: 0px;
    flex-direction: row;
    width: fit-content;
    max-width: 100%;
`;

export const SkierDetailView: React.FC<{ skierId: number }> = ({ skierId }) => {
    const [skier, setSkier] = useState<Skier | undefined>(undefined);

    useEffect(() => {
        if (skier !== undefined) return;
        setStateAsync(setSkier, getSkierById(skierId));
    }, [skier, skierId]);

    return (
        <BackLinkWrapper url="/skier" backText="Zurück zur Fahrerübersicht">
            <SkierGrid>
                {skier && (
                    <SkierDetailCard>
                        {skier.imageUrl && (
                            <img alt="Skier" src={skier.imageUrl} />
                        )}
                        <SkierDetailPanel skier={skier} />
                    </SkierDetailCard>
                )}
            </SkierGrid>
        </BackLinkWrapper>
    );
};
