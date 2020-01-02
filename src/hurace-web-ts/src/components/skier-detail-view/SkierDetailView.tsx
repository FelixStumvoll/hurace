import React, { useState, useEffect } from 'react';
import { setStateAsync } from '../../common/stateSetter';
import { getSkierById } from '../../api';
import { Skier } from '../../interfaces/Skier';
import styled from 'styled-components';
import { SkierDetailPanel } from './SkierDetailPanel';
import { GridAreaProps } from '../../interfaces/GridAreaProps';

const SkierGrid = styled.div`
    display: grid;
    grid-template-areas:
        'img detailPanel'
        'results results';
    grid-template-columns: auto 1fr;
    column-gap: 10px;
    width: 100%;
    height: 100%;
`;

const SkierImage = styled.img`
    grid-area: img;
`;

const GridAreaWrapper = styled.div<GridAreaProps>`
    grid-area: ${props => props.gridArea};
`;

export const SkierDetailView: React.FC<{ skierId: number }> = ({ skierId }) => {
    const [skier, setSkier] = useState<Skier | undefined>(undefined);

    useEffect(() => {
        setStateAsync(setSkier, getSkierById(skierId));
    }, [skier, skierId]);

    return (
        <SkierGrid>
            {skier && (
                <>
                    {skier.imageUrl && (
                        <SkierImage alt="Skier" src={skier.imageUrl} />
                    )}
                    <GridAreaWrapper gridArea="">
                        <SkierDetailPanel skier={skier} />
                    </GridAreaWrapper>
                </>
            )}
        </SkierGrid>
    );
};
